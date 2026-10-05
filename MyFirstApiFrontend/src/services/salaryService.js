javascript
import axios from "axios";

const API_URL = "https://localhost:7160/api/Salary";

// ==========================================
// GET SALARY BY EMPLOYEE ID
// ==========================================
export const getSalaryByEmployeeId = async (employeeId) => {

    const token = localStorage.getItem("token");

    const response = await axios.get(
        `${API_URL}/Employee/${employeeId}`,
        {
            headers: {
                Authorization: `Bearer ${token}`
            }
        }
    );

    return response.data;
};


// ==========================================
// ADD SALARY
// ==========================================
export const addSalary = async (salaryData) => {

    const token = localStorage.getItem("token");

    const response = await axios.post(
        `${API_URL}/AddSalary`,
        salaryData,
        {
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json"
            }
        }
    );

    return response.data;
};

