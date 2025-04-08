import React from "react";
import { Button, Box, Typography } from "@mui/material";


export default function MainPage() {
    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
            }}
        >
            <Typography
                variant="h3"
                sx={{
                    fontWeight: "bold",
                    color: "#0288D1",
                    marginBottom: "2rem",
                }}
            >
                ברוך הבא
            </Typography>
            <Box sx={{ display: "flex", gap: "1.5rem" }}>
                <Button
                    href="/supplier"
                    variant="contained"
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
                    כניסת ספק
                </Button>
                <Button
                    href="/login"
                    variant="contained"
                    sx={{
                        backgroundColor: "#81D4FA",
                        color: "white",
                        fontSize: "1.2rem",
                        fontWeight: "bold",
                        borderRadius: "25px",
                        padding: "1rem 3rem",
                        textTransform: "none",
                        "&:hover": {
                            backgroundColor: "#4FC3F7",
                            color: "inherit", 
                        },
                    }}
                >
                    כניסת בעל מכולת
                </Button>
            </Box>
        </Box>
    );
}
