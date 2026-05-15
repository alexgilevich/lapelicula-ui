import React from 'react';
import { BrowserRouter, Routes, Route, RouterProvider, createBrowserRouter } from 'react-router';
import App from './App';
import Home from './pages/Home';
import About from './pages/About';
import MovieCard, {loader as movieCardLoader} from './pages/MovieCard';
import MainLayout from './pages/layouts/MainLayout';

function Router() {
    const router = createBrowserRouter([
        {
            path: "/",
            element: <MainLayout />,
            children: [
                {
                    element: <Home />,
                    index: true,
                    handle: {
                        title: _ => "Pick your next movie",
                    }
                },
                {
                    path: "about",
                    element: <About />,
                    index: true,
                    handle: {
                        title: _ => "About the project",
                    }
                },
                {
                    path: "/movie/:id",
                    element: <MovieCard />,
                    loader: movieCardLoader,
                    handle: {
                        title: data => `${data.title} (${data.year})`,
                    }
                },
            ]
        }
    ]);

    return (
        <RouterProvider router={router} />
    );
}

export default Router;
