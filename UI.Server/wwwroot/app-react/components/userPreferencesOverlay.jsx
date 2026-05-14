import React, { useCallback, useEffect, useState } from 'react';
import GenrePreferencesConfigurator from './genrePreferencesConfigurator';
import { getGenrePreferences, saveGenrePreferences } from '../apiClient';

const OVERLAY_EVENTS = {
    VISIBILITY_CHANGED: 'userprefoverlay:visibilitychanged',
    VALUE_CHANGED: 'userprefoverlay:valuechanged',
};

export default function UserPreferencesOverlay({shown, onSaved}) {
    const [isReset, setIsReset] = useState(true);
    const [preferences, setPreferences] = useState({});

    // init preferred genres values from API
    useEffect(() => {
        getGenrePreferences().then(data => {
            setPreferences(data || {});
            Object.keys(data || {}).some(key => data[key] !== 0) && setIsReset(false);
        });
    }, []);

    const resetPreferences = useCallback(async () => {
        setPreferences({});
    }, []);

    const updateGenrePreference = useCallback(async (genre_key, newValue) => {
        setPreferences(prev =>  ({ ...prev, [genre_key]: newValue ?? 0 }));
        setIsReset(false);
    }, []);


    const savePreferences = useCallback(async () => {
        try {
            await saveGenrePreferences(preferences);
            onSaved?.();
        } catch (ex) {
            console.error('Failed to save preferences', ex);
        }
    }, [preferences]);
    
    return (
        <div className={`preferences-overlay ${!shown ? 'hidden' : ''}`}>
            <div className="preferences-overlay__container">
                <h1 className="preferences-overlay__title">How would you rate these movie genres?</h1>
                <div data-permanent>
                    <div className="preferences-overlay__body">
                        <GenrePreferencesConfigurator value={preferences} onChange={updateGenrePreference} />
                    </div>
                </div>
                <div className="preferences-overlay__actions">
                    <button className="button button_secondary" style={{ marginRight: '2vh' }} onClick={resetPreferences}>Reset</button>
                    {!isReset && (
                        <button className="button" onClick={savePreferences}>Save &amp; continue</button>
                    )}
                </div>
            </div>
        </div>
    );
}
