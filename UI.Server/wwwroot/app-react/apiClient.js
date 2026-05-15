const BASE_URL = '/api/user';

export async function getGenrePreferences() {
    const response = await fetch(`${BASE_URL}/genrePreferences`);
    if (response.status === 204) return {};
    if (!response.ok) throw new Error('Failed to fetch genre preferences');
    return response.json();
}

export async function saveGenrePreferences(preferences) {
    const response = await fetch(`${BASE_URL}/genrePreferences`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(preferences),
    });
    if (!response.ok) throw new Error('Failed to save genre preferences');
}

export async function getMovieById(movieId) {
    const response = await fetch(`/api/movie/${movieId}`);
    if (response.status === 404) return null;
    if (!response.ok) throw new Error('Failed to fetch movie');
    return response.json();
}
