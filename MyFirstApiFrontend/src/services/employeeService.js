import axios from "axios";

const API_URL = "https://localhost:7160/api/Employee";


// =====================================================
// GET TOKEN
// =====================================================

const getToken = () => {
    return localStorage.getItem("token");
};


// =====================================================
// GET ALL EMPLOYEES
// =====================================================

export const getAllEmployees = async () => {

    const response = await axios.get(
        `${API_URL}/GetAllEmployees`,
        {
            headers: {
                Authorization: `Bearer ${getToken()}`
            }
        }
    );

    return response.data;
};


// =====================================================
// GET EMPLOYEE BY ID
// =====================================================

export const getEmployeeById = async (id) => {

    const response = await axios.get(
        `${API_URL}/GetEmployeeById/${id}`,
        {
            headers: {
                Authorization: `Bearer ${getToken()}`
            }
        }
    );

    return response.data;
};


// =====================================================
// CREATE EMPLOYEE
// =====================================================

export const createEmployee = async (employee) => {

    const response = await axios.post(
        `${API_URL}/CreateEmployee`,
        employee,
        {
            headers: {
                Authorization: `Bearer ${getToken()}`,
                "Content-Type": "application/json"
            }
        }
    );

    return response.data;
};

export const updateEmployee = async (employee) => {
    const token = localStorage.getItem("token");

    const response = await axios.put(
        `${API_URL}/UpdateEmployee`,
        employee,
        {
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        }
    );

    return response.data;
};


export const deleteEmployee = async (id) => {
    const token = localStorage.getItem("token");

    const response = await axios.delete(
        `${API_URL}/DeleteEmployee?id=${id}`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );

    return response.data;
};