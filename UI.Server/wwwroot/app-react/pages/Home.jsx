import React, { useCallback, useEffect, useRef, useState } from 'react';
import MovieSlideshow from '../components/movieSlideshow';
import UserPreferencesOverlay from '../components/userPreferencesOverlay';

const EVENTS = {
    NO_COOKIE_FOUND: 'moviesshowcase:nocookie',
    VISIBILITY_CHANGED: 'userprefoverlay:visibilitychanged',
    VALUE_CHANGED: 'userprefoverlay:valuechanged',
};

export default function Home() {
    const [showMovies, setShowMovies] = useState(true);
    const movieSlideshowRef = useRef(null);

    const showPreferences = useCallback(() => {
        setShowMovies(false);
    }, []);

    const reloadMovies = () => {
        setShowMovies(true);
        movieSlideshowRef.current?.refresh();
    };

    useEffect(() => {
        const noCookieFoundHandler = () => {
            setShowMovies(false);
        };
        window.addEventListener(EVENTS.NO_COOKIE_FOUND, noCookieFoundHandler);
        return () => {
            window.removeEventListener(EVENTS.NO_COOKIE_FOUND, noCookieFoundHandler);
        };
    }, []);

    return (
        <>
            <div className={`content-wrapper ${showMovies ? '' : 'hidden'}`}>
                <section className="hero">
                    <h1>You might like <span className="highlight">these movies</span></h1>
                    <p>A carefully curated collection of movies selected just for you.</p>
                </section>

                <MovieSlideshow ref={movieSlideshowRef} />

                <div className="actions-panel">
                    <button className="button" onClick={showPreferences}>Adjust your preferences</button>
                </div>
            </div>
            <UserPreferencesOverlay shown={!showMovies} onSaved={reloadMovies} />
        </>
    );
}
