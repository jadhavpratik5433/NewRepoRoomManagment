import { useState } from "react";
import { registerUser } from "../services/authService";
import "../css/style.css";

function Register({ onBackToLogin }) {

    const [name, setName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");


    const handleRegister = async (e) => {

        e.preventDefault();

        setError("");
        setSuccess("");


        if (!name || !email || !password) {

            setError(
                "Please fill all the details"
            );

            return;
        }


        try {

            setLoading(true);


            const result = await registerUser(
                name,
                email,
                password
            );


            console.log(
                "Register Response:",
                result
            );


            if (result?.status === true) {

                setSuccess(
                    result.message ||
                    "Registration Successful!"
                );


                setName("");
                setEmail("");
                setPassword("");


                setTimeout(() => {

                    onBackToLogin();

                }, 1500);

            }
            else {

                setError(
                    result?.message ||
                    "Registration Failed!"
                );

            }

        }
        catch (error) {

            console.error(
                "Register Error:",
                error
            );


            if (error.response) {

                setError(
                    error.response.data?.message ||
                    "Registration Failed!"
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

                <h1>
                    MyFirstApi
                </h1>

                <h2>
                    Register
                </h2>


                {/* ERROR */}

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


                {/* SUCCESS */}

                {success && (

                    <div
                        style={{
                            color: "green",
                            background: "#dcfce7",
                            padding: "10px",
                            borderRadius: "6px",
                            marginBottom: "15px"
                        }}
                    >
                        {success}
                    </div>

                )}


                <form onSubmit={handleRegister}>


                    {/* NAME */}

                    <div className="form-group">

                        <label>
                            Name
                        </label>

                        <input
                            type="text"
                            placeholder="Enter Name"
                            value={name}
                            onChange={(e) =>
                                setName(e.target.value)
                            }
                        />

                    </div>


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


                    {/* REGISTER */}

                    <button
                        type="submit"
                        className="login-button"
                        disabled={loading}
                    >

                        {loading
                            ? "Registering..."
                            : "Register"
                        }

                    </button>


                    <br />
                    <br />


                    {/* BACK TO LOGIN */}

                    <button
                        type="button"
                        className="secondary-button"
                        style={{
                            width: "100%"
                        }}
                        onClick={onBackToLogin}
                    >
                        Back to Login
                    </button>

                </form>

            </div>

        </div>
    );
}

export default Register;