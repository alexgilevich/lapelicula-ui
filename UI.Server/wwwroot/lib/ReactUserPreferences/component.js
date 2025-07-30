import React, { useEffect, useState, useReducer } from 'react';
import ReactDOM from 'react-dom/client'
import Rating from '@mui/material/Rating';
import {Grid} from "@mui/material";
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import createCache from '@emotion/cache';
import { CacheProvider } from '@emotion/react';
import Cookies from 'js-cookie';
import { motion, AnimatePresence } from "motion/react"
import { ThemeProvider, createTheme } from '@mui/material/styles';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';

let objectReference = null;
let preferences = {};

export function renderComponent(containerId) {
    const root = ReactDOM.createRoot(document.getElementById(containerId));
    initializePreferences();
    root.render(<ReactUserPreferences />);
}

export function persistBlazorObject(ref) {
    objectReference = ref;
}

let triggerVisibility = null;
let rebuild = null;

export function triggerPreferences() {
    triggerVisibility?.();
}

export function resetPreferences() {
    if (objectReference) {
        preferences = {};
        objectReference.invokeMethod('ShowSaveButton', false);
        Cookies.remove("my_prefs");
        preferences = objectReference.invokeMethod('Decode', ''); // default value
        rebuild && rebuild();
    } else {
        console.error('Blazor object not found');
    }
}

function updatePreferences(genreName, value) {
    if (objectReference) {
        preferences[genreName] = value;
        let encodedCookieValue = objectReference.invokeMethod('Encode', preferences);
        Cookies.set("my_prefs", encodedCookieValue, { expires: 365 })
        objectReference.invokeMethod('ShowSaveButton', true);
    } else {
        console.error('Blazor object not found');
    }
}

function initializePreferences() {
    if (objectReference) {
        let encodedCookieValue = Cookies.get("my_prefs")
        preferences = objectReference.invokeMethod('Decode', encodedCookieValue);
        if (!encodedCookieValue) {
            resetPreferences();
        }
    } else {
        console.error('Blazor object not found');
    }
}


const keyToGenreName = {
    action: "Action",
    adventure: "Adventure",
    animation: "Animation",
    comedy: "Comedy",
    crime: "Crime",
    documentary: "Documentary",
    drama: "Drama",
    fantasy: "Fantasy",
    film_noir: "Film-Noir",
    horror: "Horror",
    kids: "Kids",
    musical: "Musical",
    mystery: "Mystery",
    romance: "Romance",
    sci_fi: "Sci-Fi",
    thriller: "Thriller",
    war: "War",
    western: "Western"
};

function createEmotionCache() {
    return createCache({ key: `mui${
        Array.from(Math.random().toString().slice(2, 7)).map(i => String.fromCharCode(97 + Number.parseInt(i))).join('')
    }`, prepend: true })
}

function ReactUserPreferences() {
    const [cache, setCache] = useState(createEmotionCache());
    useEffect(() => {
        Blazor.addEventListener('enhancedload', () => {
            // Reset cache → forces reinjection
            setCache(createEmotionCache());
        });
        
    }, []);
    
    const darkTheme = createTheme({
        palette: {
            mode: 'dark',
        },
    });
    
    const fadeUpVariants = {
        hidden: { opacity: 0, y: 20 },
        visible: { opacity: 1, y: 0 },
    };
    const [isVisible, setIsVisible] = useState(false)
    const [, forceUpdate] = useReducer((x) => x + 1, 0);
    
    useEffect(() => {
        triggerVisibility = function () { setIsVisible(isVisible => !isVisible); };
        rebuild = function () { forceUpdate(); };
    }, []);
    
    return (
        <AnimatePresence initial={true} mode="wait">
            {isVisible ? (<motion.div initial="hidden" animate="visible" exit="hidden" variants={fadeUpVariants} transition={{ duration: 0.8, ease: "easeOut" }}>
                <CacheProvider value={cache}>
                    <ThemeProvider theme={darkTheme}>
                        <Grid container spacing={{ xs: 1, md: 2 }} columns={{ xs: 2, md: 4, xl: 8 }}>
                        {Object.keys(preferences).map((genre_key, index) => (
                            <Grid key={index} size={{ xs: 2, sm: 2, md: 2 }}>
                                <Box component="section">
                                    <Typography component="legend">{keyToGenreName[genre_key]}</Typography>
                                    <Rating name="half-rating" 
                                            value={preferences[genre_key]} 
                                            precision={0.5} size="large" 
                                            onChange={(event, newValue) => {
                                                updatePreferences(genre_key, newValue);
                                                forceUpdate();
                                            }} />
                                </Box>
                            </Grid>
                        ))}
                        </Grid>
                    </ThemeProvider>
                </CacheProvider>
            </motion.div>) : null}
        </AnimatePresence>)
}