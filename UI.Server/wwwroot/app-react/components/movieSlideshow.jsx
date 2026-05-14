import React, { useCallback, useEffect, useImperativeHandle, useRef, useState } from 'react';
import Glide from '@glidejs/glide'
import "@glidejs/glide/dist/css/glide.core.min.css";

const API_EVENTS = {
    NO_COOKIE_FOUND: 'moviesshowcase:nocookie',
};

export default function MovieSlideshow({ props, ref }) {
    const [recommendations, setRecommendations] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [modelNotTrained, setModelNotTrained] = useState(false);
    const glideRef = useRef(null);

    const fetchRecommendations = useCallback(async () => {
        try {
            setIsLoading(true);
            setModelNotTrained(false);
            const response = await fetch('/api/recommendations');

            if (response.status === 204) {
                window.dispatchEvent(new CustomEvent(API_EVENTS.NO_COOKIE_FOUND));
                return;
            }

            if (response.status === 503) {
                setModelNotTrained(true);
                return;
            }

            if (!response.ok) {
                const text = await response.text();
                console.error('Failed to get recommendations:', text);
                return;
            }

            const data = await response.json();
            setRecommendations(data || []);
        } catch (ex) {
            console.error('Failed to get recommendations', ex);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchRecommendations();
    }, []);

    useImperativeHandle(ref, () => ({
        refresh: fetchRecommendations,
    }), []);

    useEffect(() => {
        if (recommendations.length > 0 && glideRef.current) {
            renderComponent('.glide');
        }
    }, [recommendations]);

    return (
        <div className="recommendations-container">
            <div className={`glide ${isLoading ? 'glide_loading' : ''}`} ref={glideRef}>
                {modelNotTrained ? (
                    <p className="glide_model-not-trained">Model is not trained yet. Try again later...</p>
                ) : (
                    <div>
                        <div className="gradient-overlay overlay-left"></div>
                        <div className="gradient-overlay overlay-right"></div>

                        <div className="glide__track" data-glide-el="track">
                            <ul className="glide__slides">
                                {recommendations.map((rec) => (
                                    <li key={rec.movie.id} className="glide__slide" style={{ width: '300px' }}>
                                        <a
                                            className="glide__slide-image-container"
                                            href={`https://www.themoviedb.org/movie/${rec.movie.tmdbId}`}
                                            target="_blank"
                                            rel="noopener noreferrer"
                                        >
                                            <img src={rec.movie.posterUri} alt={rec.movie.description} />
                                            <div className="glide__slide-image-overlay">
                                                <p className="glide__slide-description">{rec.movie.description}</p>
                                                <p className="glide__slide-genres">{rec.movie.rawGenres.join(', ')}</p>
                                            </div>
                                            <div className="glide__slide-rating">{rec.rating.toFixed(1)}</div>
                                        </a>
                                        <p className="glide__slide-title">
                                            {rec.movie.title} ({rec.movie.year})
                                        </p>
                                    </li>
                                ))}
                            </ul>
                        </div>

                        <div className="glide__arrows" data-glide-el="controls">
                            <button className="glide__arrow glide__arrow--left" data-glide-dir="<">&#8249;</button>
                            <button className="glide__arrow glide__arrow--right" data-glide-dir=">">&#8250;</button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}



let glideInstance = null;
function renderComponent(selector) 
{
    const GlideTouchSupport = (function () {
        const events = {
            slideClick: e => {
                const slide = e.currentTarget;
                if (slide._slideClickTimeout)
                    clearTimeout(slide._slideClickTimeout);
                
                const shown = slide.classList.contains('glide__slide_show-overlay');
                if (!shown) {
                    e.preventDefault();
                    slide.classList.add('glide__slide_show-overlay');
                    // timeout to hide the overlay 
                    slide._slideClickTimeout = setTimeout(() => slide.classList.remove('glide__slide_show-overlay'), 7000);
                }
            }
        }
        
        return {
            build: function () {
                // only on touch-supporting devices
                if (!('ontouchstart' in window))
                    return;
                
                let slideNodes = document.querySelectorAll(selector + ' .glide__slide')
                slideNodes.forEach(slide => {
                    slide.addEventListener('click', events.slideClick);
                });
                document.addEventListener('touchstart', ev => {
                    // don't do anything if it was a click on the slide
                    const closestSlideNode = ev.target.closest('.glide__slide');
                    if (closestSlideNode) 
                        return;
                    
                    // otherwise, remove all active overlays from all slides
                    slideNodes.forEach(slide => {
                        slide.classList.remove('glide__slide_show-overlay');
                        if (slide._slideClickTimeout) 
                            clearTimeout(slide._slideClickTimeout);
                    });
                });
            },
            destroy: function (){
                let slideNodes = document.querySelectorAll(selector + ' .glide__slide')
                slideNodes.forEach(slide => {
                    slide.removeEventListener('click', events.slideClick);
                });
                document.removeEventListener('touchstart', events.slideClick);
            }
        }
    })();
    
    
    if (glideInstance) {
        try {
            glideInstance.destroy();
            GlideTouchSupport.destroy();
            
        } catch (error) {}
    }
    
    glideInstance = new Glide(selector, {
        type: 'carousel',
        perView: 3,
        focusAt: 'center',
        gap: 24,
        autoplay: 5000,
        breakpoints: {
            4000: {
                perView: 15
            },
            2500: {
                perView: 11
            },
            2000: {
                perView: 7
            },
            1500: {
                perView: 5
            },
            1200: {
                perView: 3
            },
            750: {
                perView: 2
            },
            500: {
                perView: 1
            }
        }
    });
    
    glideInstance.on('mount.after', function() {
        GlideTouchSupport.build();
    });

    glideInstance.mount();
}