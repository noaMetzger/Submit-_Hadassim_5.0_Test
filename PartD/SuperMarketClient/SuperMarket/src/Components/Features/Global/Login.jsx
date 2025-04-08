import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { TextField, Button, Box, Typography, InputAdornment, Grid } from "@mui/material";
import { Email, Lock } from "@mui/icons-material";
import axios from "axios";

export default function Login() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        email: "",
        password: ""
    });
    const [errors, setErrors] = useState({});
    const [serverMessage, setServerMessage] = useState(""); 

    //מעדכנת את נתוני ההתחברות בעת לחיצה
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    //פונקציית ולידציה לפרטי ההתחברות
    const validateForm = () => {
        let formErrors = {};
        if (!formData.email) formErrors.email = "אימייל חובה";
        if (!formData.password) formErrors.password = "סיסמה חובה";
        return formErrors;
    };

    const handleSubmit = async (e) => {
        e.preventDefault(); // כדי שלא יתרנדר 
        const formErrors = validateForm();
        setErrors(formErrors);
        setServerMessage("");

        if (Object.keys(formErrors).length === 0) {
            //מנסה להתחבר לבעל מכולת ואם לא הצלחתי אז לספק ואם לא הצלחתי אז שגיאה ולא קיים כזה משתמש
            try {
                const { data: userData } = await axios.post("https://localhost:7270/api/User/login", formData);
                if (userData) {
                    localStorage.removeItem("token");
                    localStorage.setItem("token", userData);
                    navigate("/home");
                }
            } catch (userError) {

                try {
                    const { data: supplierData } = await axios.post("https://localhost:7270/api/Supplier/login", formData);
                    if (supplierData) {
                        localStorage.removeItem("token");
                        localStorage.setItem("token", supplierData);
                        navigate("/home");
                    }
                } catch (supplierError) {
                    setServerMessage("שגיאה בכניסה, נסה שוב מאוחר יותר");
                }
            }
        }
    }

        return (
            <div className="LoginForm">
                <Box
                    component="form"
                    onSubmit={handleSubmit}
                    sx={{
                        maxWidth: 600,
                        mx: "auto",
                        p: 4,
                        borderRadius: 5,
                        backgroundColor: "white",
                        position: "relative",
                        zIndex: 2,
                    }}
                >
                    <Typography
                        variant="h5"
                        gutterBottom
                        textAlign="center"
                        fontWeight="bold"
                        color="#1E90FF"
                    >
                        התחברות
                    </Typography>

                    {serverMessage && (
                        <Typography
                            variant="body1"
                            color="error"
                            textAlign="center"
                            gutterBottom
                        >
                            {serverMessage}
                        </Typography>
                    )}

                    <Grid container spacing={2}>
                        {[
                            { label: "מייל", name: "email", type: "email", icon: <Email /> },
                            { label: "סיסמא", name: "password", type: "password", icon: <Lock /> },
                        ].map(({ label, name, type, icon }) => (
                            <Grid item xs={12} key={name}>
                                <TextField
                                    label={label}
                                    name={name}
                                    type={type}
                                    value={formData[name]}
                                    onChange={handleInputChange}
                                    fullWidth
                                    error={!!errors[name]}
                                    helperText={errors[name] || ""}
                                    InputProps={{
                                        startAdornment: (
                                            <InputAdornment position="start" sx={{ color: "#1E90FF" }}>
                                                {icon}
                                            </InputAdornment>
                                        ),
                                    }}
                                />
                            </Grid>
                        ))}
                    </Grid>

                    <Button
                        type="submit"
                        variant="contained"
                        sx={{
                            mt: 3,
                            backgroundColor: "#1E90FF",
                            color: "white",
                            width: "50%",
                            py: 1,
                            mx: "auto",
                            display: "block",
                            "&:hover": { backgroundColor: "#4682B4" },
                        }}
                    >
                        התחבר
                    </Button>
                </Box>
            </div>
        );
    }

