import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router';
import App from './App';
import Home from './pages/Home';
import About from './pages/About';
import MainLayout from './pages/layouts/MainLayout';

function Router() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<MainLayout />}>
                    <Route index element={<Home />} />
                    <Route path="/about" element={<About />} />
                </Route>
            </Routes>
        </BrowserRouter>
    );
}

export default Router;
