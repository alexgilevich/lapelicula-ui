import { useMatches } from 'react-router';
import { useEffect } from 'react';

export default function PageTitle() {
    const matches = useMatches()
    const { handle, data } = matches[matches.length - 1]
    const title = handle && handle.title && handle.title(data);

    useEffect(() => {
        if (title) {
            document.title = title + " - La Pelicula";
        }
    }, [title])

    return (
    <>
        </>
    )
}