import { useState } from "react";

import {
    loginUser,
    generateQrCode,
    verifyOtp
} from "../services/authService";

import "../css/style.css";

function Login({ onLogin, onRegister }) {

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [qrCode, setQrCode] = useState("");
    const [otp, setOtp] = useState("");

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    // =========================
    // LOGIN
    // =========================
    const handleLogin = async (e) => {

        e.preventDefault();

        setError("");

        if (!email || !password) {
            setError("Please enter Email and Password");
            return;
        }

        try {

            setLoading(true);

            // Login API call
            const result = await loginUser(email, password);

            console.log("Login Response:", result);

            if (result?.data?.token) {

                // Store token
                localStorage.setItem(
                    "token",
                    result.data.token
                );

                // Generate QR Code
                const qrResult = await generateQrCode(email);

                console.log(
                    "QR Response:",
                    qrResult
                );

                if (qrResult?.data) {

                    // Show QR Code
                    setQrCode(qrResult.data);

                }
                else {

                    setError(
                        "QR Code generation failed"
                    );
                }

            }
            else {

                setError(
                    result?.message ||
                    "Token not found"
                );
            }

        }
        catch (error) {

            console.error(
                "Login Error:",
                error
            );

            if (error.response) {

                setError(
                    error.response.data?.message ||
                    "Login Failed!"
                );

            }
            else {

                setError(
                    "Unable to connect to API!"
                );
            }

        }
        finally {

            setLoading(false);

        }
    };


    // =========================
    // VERIFY OTP
    // =========================
    const handleVerifyOtp = async () => {

        setError("");

        // Check OTP
        if (!otp || otp.length !== 6) {

            setError(
                "Please enter 6 digit OTP"
            );

            return;
        }

        try {

            setLoading(true);

            // Verify OTP API call
            const result = await verifyOtp(
                email,
                otp
            );

            console.log(
                "Verify OTP Response:",
                result
            );

            if (result?.data) {

                alert(
                    "OTP Verified Successfully!"
                );

                // Now allow login
                onLogin();

            }
            else {

                setError(
                    result?.message ||
                    "OTP verification failed"
                );
            }

        }
        catch (error) {

            console.error(
                "OTP Verification Error:",
                error
            );

            if (error.response) {

                setError(
                    error.response.data?.message ||
                    "Invalid OTP"
                );

            }
            else {

                setError(
                    "Unable to connect to API!"
                );
            }

        }
        finally {

            setLoading(false);

        }
    };


    return (

        <div className="login-container">

            <div className="login-card">

                <h1>MyFirstApi</h1>

                <h2>Login</h2>


                {/* ERROR MESSAGE */}
                {error && (

                    <div
                        style={{
                            color: "red",
                            background: "#fee2e2",
                            padding: "10px",
                            borderRadius: "6px",
                            marginBottom: "15px"
                        }}
                    >

                        {error}

                    </div>

                )}


                {/* LOGIN FORM */}
                <form onSubmit={handleLogin}>

                    {/* EMAIL */}
                    <div className="form-group">

                        <label>
                            Email
                        </label>

                        <input
                            type="email"
                            placeholder="Enter Email"
                            value={email}
                            onChange={(e) =>
                                setEmail(e.target.value)
                            }
                        />

                    </div>


                    {/* PASSWORD */}
                    <div className="form-group">

                        <label>
                            Password
                        </label>

                        <input
                            type="password"
                            placeholder="Enter Password"
                            value={password}
                            onChange={(e) =>
                                setPassword(e.target.value)
                            }
                        />

                    </div>


                    {/* LOGIN BUTTON */}
                    <button
                        type="submit"
                        className="login-button"
                        disabled={loading}
                    >

                        {loading
                            ? "Logging in..."
                            : "Login"
                        }

                    </button>

                </form>


                {/* ========================= */}
                {/* QR CODE + OTP SECTION */}
                {/* ========================= */}

                {qrCode && (

                    <div
                        style={{
                            textAlign: "center",
                            marginTop: "25px",
                            padding: "20px",
                            borderTop:
                                "1px solid #ddd"
                        }}
                    >

                        <h3>
                            Scan QR Code
                        </h3>


                        {/* QR CODE */}
                        <img
                            src={`data:image/png;base64,${qrCode}`}
                            alt="QR Code"
                            style={{
                                width: "220px",
                                height: "220px",
                                marginTop: "10px"
                            }}
                        />


                        <p
                            style={{
                                marginTop: "10px"
                            }}
                        >
                            Scan this QR Code using
                            your Authenticator App.
                        </p>


                        {/* OTP INPUT */}
                        <input
                            type="text"
                            placeholder="Enter 6 digit OTP"
                            value={otp}
                            maxLength={6}
                            onChange={(e) =>
                                setOtp(
                                    e.target.value
                                )
                            }
                            style={{
                                width: "100%",
                                padding: "12px",
                                marginTop: "15px",
                                border:
                                    "1px solid #ccc",
                                borderRadius: "6px",
                                boxSizing:
                                    "border-box"
                            }}
                        />


                        {/* VERIFY OTP BUTTON */}
                        <button
                            type="button"
                            className="login-button"
                            style={{
                                width: "100%",
                                marginTop: "10px"
                            }}
                            onClick={handleVerifyOtp}
                            disabled={loading}
                        >

                            {loading
                                ? "Verifying..."
                                : "Verify OTP"
                            }

                        </button>

                    </div>

                )}


                {/* REGISTER */}
                {!qrCode && (

                    <div
                        style={{
                            textAlign: "center",
                            marginTop: "20px"
                        }}
                    >

                        <span>
                            Don't have an account?
                        </span>

                        <br />

                        <button
                            type="button"
                            className="secondary-button"
                            style={{
                                marginTop: "10px",
                                width: "100%"
                            }}
                            onClick={onRegister}
                        >

                            Create New Account

                        </button>

                    </div>

                )}

            </div>

        </div>

    );
}

export default Login;