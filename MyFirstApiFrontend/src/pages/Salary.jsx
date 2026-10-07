import React, { useState } from "react";
import { getSalaryByEmployeeId } from "../services/salaryService";

const Salary = () => {
    const [employeeId, setEmployeeId] = useState("");
    const [salaryDetails, setSalaryDetails] = useState([]);
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    const handleGetSalary = async () => {
        if (!employeeId.trim()) {
            setError("Please enter Employee ID");
            return;
        }

        try {
            setLoading(true);
            setError("");
            setSalaryDetails([]);

            const response = await getSalaryByEmployeeId(employeeId);

            if (response.status && response.data) {
                setSalaryDetails(response.data);
            } else {
                setError(response.message || "Salary Details Not Found");
            }
        } catch (error) {
            console.error(error);

            if (error.response?.data?.message) {
                setError(error.response.data.message);
            } else {
                setError("Salary Details Not Found");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div style={{ padding: "30px" }}>
            <h2>Salary Details</h2>

            <div style={{ marginBottom: "20px" }}>
                <label>
                    <strong>Employee ID</strong>
                </label>

                <br />

                <input
                    type="text"
                    value={employeeId}
                    onChange={(e) => setEmployeeId(e.target.value)}
                    placeholder="Enter Employee ID"
                    style={{
                        width: "350px",
                        padding: "10px",
                        marginTop: "8px"
                    }}
                />

                <br />

                <button
                    onClick={handleGetSalary}
                    disabled={loading}
                    style={{
                        marginTop: "10px",
                        padding: "10px 20px"
                    }}
                >
                    {loading ? "Loading..." : "Get Salary"}
                </button>
            </div>

            {error && (
                <p style={{ color: "red" }}>
                    {error}
                </p>
            )}

            {salaryDetails.length > 0 && (
                <div>
                    <h3>Salary Details</h3>

                    {salaryDetails.map((salary, index) => (
                        <div
                            key={index}
                            style={{
                                border: "1px solid #ccc",
                                padding: "20px",
                                marginBottom: "15px",
                                width: "500px"
                            }}
                        >
                            <p>
                                <strong>Employee ID:</strong>{" "}
                                {salary.employeeId}
                            </p>

                            <p>
                                <strong>Salary Month:</strong>{" "}
                                {new Date(
                                    salary.salaryMonth
                                ).toLocaleDateString()}
                            </p>

                            <p>
                                <strong>Salary Date:</strong>{" "}
                                {salary.salaryDate
                                    ? new Date(
                                          salary.salaryDate
                                      ).toLocaleDateString()
                                    : "-"}
                            </p>

                            <p>
                                <strong>Payment Status:</strong>{" "}
                                {salary.paymentStatus}
                            </p>

                            <p>
                                <strong>Remarks:</strong>{" "}
                                {salary.remarks || "-"}
                            </p>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default Salary;