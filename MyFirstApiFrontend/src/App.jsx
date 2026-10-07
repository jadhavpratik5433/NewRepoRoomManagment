import { useState } from "react";

import Login from "./pages/Login";
import Register from "./pages/Register";
import Dashboard from "./pages/Dashboard";


function App() {

    const [isLoggedIn, setIsLoggedIn] =
        useState(
            !!localStorage.getItem("token")
        );


    const [showRegister, setShowRegister] =
        useState(false);


    const handleLogin = () => {

        setIsLoggedIn(true);

    };


    const handleLogout = () => {

        localStorage.removeItem("token");

        setIsLoggedIn(false);

    };


    // =========================
    // LOGGED IN
    // =========================

    if (isLoggedIn) {

        return (

            <Dashboard
                onLogout={handleLogout}
            />

        );
    }


    // =========================
    // REGISTER PAGE
    // =========================

    if (showRegister) {

        return (

            <Register
                onBackToLogin={() =>
                    setShowRegister(false)
                }
            />

        );
    }


    // =========================
    // LOGIN PAGE
    // =========================

    return (

        <Login
            onLogin={handleLogin}
            onRegister={() =>
                setShowRegister(true)
            }
        />

    );
}


export default App;