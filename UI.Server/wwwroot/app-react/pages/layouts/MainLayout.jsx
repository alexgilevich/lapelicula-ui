import React from 'react';
import './MainLayout.css';
import PageTitle from '../../components/pageTitle';
import { NavLink, Outlet } from 'react-router';

function MainLayout({ children }) {
    return (
        <>
            <PageTitle />
            <header>
                <NavLink to="/" className="logo">
                    La Pelicula
                    <div className="logo__subtext">Pick your next movie to watch</div>
                </NavLink>
                <nav>
                    <a href="https://github.com/alexgilevich/lapelicula-ui" target="_blank">GitHub</a>
                </nav>
            </header>
            <main>
                <Outlet />
            </main>
        </>
    );
}

export default MainLayout;
