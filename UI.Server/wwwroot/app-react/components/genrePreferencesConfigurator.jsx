import React, { useEffect, useState, useReducer, useCallback } from 'react';
import ReactDOM from 'react-dom/client'
import Rating from '@mui/material/Rating';
import {Grid} from "@mui/material";
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import createCache from '@emotion/cache';
import { CacheProvider } from '@emotion/react';
import { motion, AnimatePresence } from "motion/react"
import { ThemeProvider, createTheme } from '@mui/material/styles';
import '@fontsource/roboto/300.css';
import '@fontsource/roboto/400.css';
import '@fontsource/roboto/500.css';
import '@fontsource/roboto/700.css';




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

export default function GenrePreferencesConfigurator({ value, onChange }) {
    const [cache, setCache] = useState(createEmotionCache());
    const [, forceUpdate] = useReducer((x) => x + 1, 0);
    useEffect(() => {
        setCache(createEmotionCache());
    }, []);

    const darkTheme = createTheme({
        palette: { mode: 'dark' },
    });

    const fadeUpVariants = {
        hidden: { opacity: 0, y: 20 },
        visible: { opacity: 1, y: 0 },
    };

    const updateGenrePreference = useCallback((genre_key, newValue) => {
        onChange?.(genre_key, newValue);
    }, [onChange]);

    return (
        <AnimatePresence initial={true} mode="wait">
            <motion.div initial="hidden" animate="visible" exit="hidden" variants={fadeUpVariants} transition={{ duration: 0.8, ease: "easeOut" }}>
                <CacheProvider value={cache}>
                    <ThemeProvider theme={darkTheme}>
                        <Grid container columnSpacing={{ xs: 2, md: 4, lg: 6 }} rowSpacing={{ xs: 3 }} columns={{ xs: 4, md: 6 }}>
                            {Object.keys(value).map((genre_key, index) => (
                                <Grid key={index} size={{ xs: 2, sm: 2, md: 2 }}>
                                    <Box component="section">
                                        <Typography component="legend">{keyToGenreName[genre_key]}</Typography>
                                        <Rating name="half-rating"
                                                value={value[genre_key]}
                                                precision={0.5} size="large"
                                                onChange={(event, newValue) => {
                                                    updateGenrePreference(genre_key, newValue);
                                                }} />
                                    </Box>
                                </Grid>
                            ))}
                        </Grid>
                    </ThemeProvider>
                </CacheProvider>
            </motion.div>
        </AnimatePresence>
    );
}
