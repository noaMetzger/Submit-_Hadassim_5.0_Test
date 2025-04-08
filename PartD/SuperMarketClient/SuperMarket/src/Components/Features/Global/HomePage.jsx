
import { useState, useEffect } from "react";
import { Box, Typography, Grid, CircularProgress, Alert, Button, Collapse, Paper } from "@mui/material";
import axios from "axios";
import { useNavigate } from "react-router-dom";


export default function HomePage() {

    const [role, setRole] = useState(null);
    const navigate = useNavigate();

    // פונקציה לניתוח הטוקן
    const parseJwt = (token) => {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split("")
                .map((c) => `%${("00" + c.charCodeAt(0).toString(16)).slice(-2)}`)
                .join("")
        );
        return JSON.parse(jsonPayload);
    };

    useEffect(() => {
        //כך שאני אציג את האפשרויות המתאימות role מעדכנת את ה 
        const token = localStorage.getItem("token");
        if (token) {
            const decodedToken = parseJwt(token);
            setRole(decodedToken.role); 
        }
    }, []);

    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
            }}
        >
            <Typography variant="h4" sx={{ marginBottom: 2 }}>
                ברוך הבא לסופרמרקט
            </Typography>
            <Button
                variant="contained"
                color="primary"
                onClick={() => navigate("/orderList")}
                sx={{
                    backgroundColor: "#039BE5",
                    color: "white",
                    fontSize: "1.2rem",
                    fontWeight: "bold",
                    borderRadius: "25px",
                    padding: "1rem 3rem",
                    textTransform: "none",
                    marginBottom: "1rem",
                    "&:hover": {
                        backgroundColor: "#0288D1", 
                        color: "inherit", 
                    },

                }}            >
                צפייה בהזמנות
            </Button>

            {role === "grocer" && (
                <Button
                    variant="contained"
                    color="success"
                    onClick={() => navigate("/create-order")}
                    sx={{
                        backgroundColor: "#039BE5",
                        color: "white",
                        fontSize: "1.2rem",
                        fontWeight: "bold",
                        borderRadius: "25px",
                        padding: "1rem 3rem",
                        textTransform: "none",
                        "&:hover": {
                            backgroundColor: "#0288D1", 
                            color: "inherit", 
                        },

                    }}
                >
                    ביצוע הזמנה
                </Button>
            )}
        </Box>
    );
}
    


