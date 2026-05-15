import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { motion } from "motion/react";
import { getMovieById } from '../apiClient';
import { useLoaderData } from "react-router";
import './MovieCard.css';

const fadeUpVariants = {
    hidden: { opacity: 0 },
    visible: { opacity: 1, x: 0 },
};

const posterVariants = {
    hidden: { opacity: 0, x: -150 },
    visible: { ...fadeUpVariants.visible, transition: { duration: 0.6, ease: "easeOut" } },
};

const contentVariants = {
    hidden: { opacity: 0, x: 150 },
    visible: { ...fadeUpVariants.visible, transition: { duration: 0.6, delay: 0.15, ease: "easeOut" } },
};

export async function loader({ params }) {
  const { id } = params;
  return await getMovieById(id);
}

export default function MovieCard() {
    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(true);
    const movie = useLoaderData();

    useEffect(() => {
        setIsLoading(false);
    }, [id]);

    if (isLoading) {
        return <div className="movie-card loading">Loading...</div>;
    }

    if (!movie) {
        return <div className="movie-card not-found">Movie not found</div>;
    }

    const genres = movie.rawGenres || [];
    const rating = movie.ratingAverage;

    return (
        <div className="movie-card">
            <motion.div
                className="movie-card__content"
                initial="hidden"
                animate="visible"
                variants={contentVariants}
            >
                <h1 className="movie-card__title">
                    {movie.title}
                    <span className="movie-card__year"> ({movie.year})</span>
                </h1>

                { movie.tagline ? <p className="movie-card__tagline">"{movie.tagline}"</p> : <></> }

                <a
                    className="movie-card__tmdb-link"
                    href={`https://www.themoviedb.org/movie/${movie.tmdbId}`}
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    TMDB
                </a>
                &nbsp;
                <a
                    className="movie-card__tmdb-link"
                    href={`https://www.imdb.com/title/tt${("0000000" + movie.imdbId).slice(-7)}/`}
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    IMDB
                </a>

                <div className="movie-card__meta">
                    {genres.length > 0 && (
                        <div className="movie-card__genres">
                            {genres.map((genre, i) => (
                                <span key={i} className="movie-card__genre-tag">{genre}</span>
                            ))}
                            {movie.adult && (
                                <span className="movie-card__adult-tag">For Adults</span>
                            )}
                        </div>
                    )}

                    <div className="movie-card__ratings">
                        {rating > 0 && (
                            <span className="movie-card__rating" title={`Average rating: ${rating.toFixed(1)}`}>
                                Rating: {rating.toFixed(1)}
                            </span>
                        )}
                        {movie.ageRating && (
                            <span className="movie-card__age-rating" title="Age rating">
                                {movie.ageRating}
                            </span>
                        )}
                    </div>

                    {(movie.budget > 0 || movie.revenue > 0) && (
                        <div className="movie-card__financials">
                            {movie.budget > 0 && (
                                <span>Budget: ${movie.budget.toLocaleString('en-US')}</span>
                            )}
                            {movie.revenue > 0 && (
                                <span>Revenue: ${movie.revenue.toLocaleString('en-US')}</span>
                            )}
                        </div>
                    )}
                </div>

                {movie.description && (
                    <p className="movie-card__description">{movie.description}</p>
                )}
            </motion.div>

            <motion.div
                className="movie-card__poster"
                initial="hidden"
                animate="visible"
                variants={posterVariants}
            >
                <img src={movie.posterUri} alt={movie.title} />
            </motion.div>
        </div>
    );
}
