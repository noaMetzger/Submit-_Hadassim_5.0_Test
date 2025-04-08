import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { TextField, Button, Box, Typography, Grid, IconButton, InputAdornment } from "@mui/material";
import { Business, Phone, AccountCircle, Email, Lock, ShoppingCart, AttachMoney, Add } from "@mui/icons-material";
import axios from 'axios';

export default function SupplierSignUpForm() {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({ companyName: "", phoneNumber: "", agentName: "", email: "", password: "", stock: [] });
    const [currentStock, setCurrentStock] = useState({ name: "", price: "", minAmount: "" });
    const [showstockForm, setShowstockForm] = useState(false);
    const [errors, setErrors] = useState({});
    const [errors1, setErrors1] = useState("");
    const handleChange = (e) => setFormData({ ...formData, [e.target.name]: e.target.value });
    const handleStockChange = (e) => setCurrentStock({ ...currentStock, [e.target.name]: e.target.value });


    const checkEmailExists = async (email) => {
        try {
            const response = await axios.get("https://localhost:7270/api/Supplier");
            const users = response.data;

            // חפש אם יש מייל תואם
            const emailExists = users.some(user => user.email === email);
            return emailExists;
        } catch (error) {
            return "שגיאה בהתחברות לשרת";
        }
    };

    const addStock = () => {
        if (!currentStock.name || !currentStock.price || !currentStock.minAmount) return;
        setFormData({ ...formData, stock: [...formData.stock, currentStock] });
        setCurrentStock({ name: "", price: "", minAmount: "" });
        setShowstockForm(false);
    };

    const validateForm = () => {
        let err = {};
        if (!formData.companyName) err.companyName = "חובה";
        if (!/^\d{10}$/.test(formData.phoneNumber)) err.phoneNumber = "מספר לא תקין";
        if (!formData.email.includes("@")) err.email = "אימייל לא תקין";
        if (formData.password.length < 6) err.password = "לפחות 6 תווים";
        if (formData.stock.length === 0) err.stock = "חובה להוסיף סחורה";
        if (!formData.agentName) err.agentName = "חובה";
        return err;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const err = validateForm();
        setErrors(err);

        if (Object.keys(err).length === 0) {
            debugger
            try {
                // בדוק אם המייל כבר קיים
                const emailExists = await checkEmailExists(formData.email);
                if (emailExists == "שגיאה בהתחברות לשרת") {
                    setErrors1("שגיאה בהתחברות לשרת");
                    return; // אל תמשיך אם יש שגיאה בהתחברות לשרת
                }
                if (emailExists) {
                    setErrors1("המייל כבר קיים במערכת");
                    return; // אל תמשיך אם המייל קיים
                }
                const supplierResponse = await axios.post("https://localhost:7270/api/Supplier", {
                    companyName: formData.companyName,
                    phoneNumber: formData.phoneNumber,
                    agentName: formData.agentName,
                    email: formData.email,
                    password: formData.password,
                    role: "supplier",
                });

                if (supplierResponse.status === 200) {
                    const supplierId = supplierResponse.data.id;

                    try {
                        for (let stock of formData.stock) {
                            await axios.post("https://localhost:7270/api/Stock", {
                                supplierId: supplierId,
                                name: stock.name,
                                price: stock.price,
                                minAmount: stock.minAmount,
                            });
                            navigate("/login");

                        }
                    } catch (error) {
                        setErrors1("הייתה שגיאה בהתחברות לשרת.");
                    }

                } else {
                    setErrors1("הייתה שגיאה בהרשמה, נסה שוב.");
                }
            } catch (error) {
                setErrors1("הייתה שגיאה בהתחברות לשרת.");
            }
        }
    };

    return (
        <Box component="form" onSubmit={handleSubmit} sx={{ maxWidth: 400, mx: "auto", p: 2, display: "flex", flexDirection: "column", justifyContent: "center" }}>
            <Typography variant="h5" textAlign="center">הרשמת ספקים</Typography>
            <Grid container spacing={2} justifyContent="center" alignItems="center">
                {[{ label: "שם חברה", name: "companyName", icon: <Business /> },
                { label: "מספר טלפון", name: "phoneNumber", icon: <Phone /> },
                { label: "שם נציג", name: "agentName", icon: <AccountCircle /> },
                { label: "אימייל", name: "email", icon: <Email /> },
                { label: "סיסמה", name: "password", type: "password", icon: <Lock /> }]
                    .map(({ label, name, type = "text", icon }) => (
                        <Grid item xs={12} key={name}>
                            <TextField fullWidth label={label} name={name} type={type} value={formData[name]} onChange={handleChange} error={!!errors[name]} helperText={errors[name]}
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
            <Button onClick={() => setShowstockForm(true)} variant="outlined" fullWidth sx={{ mt: 2 }}>
                הוסף סחורה
            </Button>

            {showstockForm && (
                <>
                    <Typography sx={{ mt: 2 }}>פרטי סחורה</Typography>
                    <Grid >
                        <Grid container spacing={1} justifyContent="center" alignItems="center">
                            {[{ label: "שם מוצר", name: "name", icon: <ShoppingCart /> },
                            { label: "מחיר", name: "price", type: "number", icon: <AttachMoney /> },
                            { label: "כמות מינימלית", name: "minAmount", type: "number", icon: <AttachMoney /> }]
                                .map(({ label, name, type = "text", icon }) => (
                                    <Grid item xs={4} key={name}>
                                        <TextField fullWidth label={label} name={name} type={type} value={currentStock[name]} onChange={handleStockChange} />
                                    </Grid>
                                ))}
                        </Grid>
                        <Grid item xs={1}>
                            <IconButton onClick={addStock}><Add /></IconButton>
                        </Grid>
                    </Grid>
                </>
            )}

            {errors.stock && <Typography color="error">{errors.stock}</Typography>}
            {errors1 && <Typography color="error">{errors1}</Typography>}

            <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }}>הרשמה</Button>
        </Box>
    );
}
