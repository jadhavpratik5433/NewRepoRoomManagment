import axios from "axios";

const API_URL = "https://localhost:7160/api/Auth";

export const loginUser = async (email, password) => {
    const response = await axios.post(
        `${API_URL}/Login`,
        {
            email: email,
            password: password
        }
    );

    return response.data;
};

export const registerUser = async (
    name,
    email,
    password
) => {
    const response = await axios.post(
        `${API_URL}/Register`,
        {
            name: name,
            email: email,
            password: password
        }
    );

    return response.data;
};

export const generateQrCode = async (email) => {
    const response = await axios.post(
        `${API_URL}/GenerateQrCode`,
        null,
        {
            params: {
                email: email
            }
        }
    );

    return response.data;
};

export const verifyOtp = async (email, otp) => {
    const response = await axios.post(
        `${API_URL}/VerifyOTP`,
        null,
        {
            params: {
                email: email,
                otp: otp
            }
        }
    );

    return response.data;
};