import React, { useState, useEffect } from "react";
import axios from "axios";
import { Button, Typography, TextField } from "@mui/material";

export default function ChoseStocks({ supplierId, onSelectStocks }) {
    const [stocks, setStocks] = useState([]);
    const [selectedStocks, setSelectedStocks] = useState([]);
    const [error, setError] = useState("");
    const [totalPrice, setTotalPrice] = useState(0);

    useEffect(() => {
        const fetchStocks = async () => {
            try {
                const response = await axios.get(`https://localhost:7270/api/Supplier/${supplierId}`);
                setStocks(response.data.stocks);
            }
            catch (error) {
                setError("שגיאה בהתחברות לשרת");
            }
        };

        if (supplierId) fetchStocks();
    }, []);

    const handleStockSelection = (productId, quantity) => {
        const selectedProduct = stocks.find((stock) => stock.id === productId);
        if (quantity < selectedProduct.minAmount) {
            const name = selectedProduct.name;
            const minAmount = selectedProduct.minAmount;
            setError(`הכמות המינימלית עבור ${name} היא ${minAmount}.`);
        } else {
            setError("");
            const updatedStock = {
                productId: selectedProduct.id,
                name: selectedProduct.name,
                quantity: quantity,
                price: selectedProduct.price,
            };

            setTotalPrice((prevTotal) =>
                prevTotal +
                (quantity * selectedProduct.price)
                - (selectedStocks.find((s) => s.productId === productId)?.quantity || 0) *
                selectedProduct.price
            );
            setSelectedStocks((prev) => {
                const existingIndex = prev.findIndex(
                    (stock) => stock.productId === productId
                );
                if (existingIndex !== -1) {
                    const updatedStocks = [...prev];
                    updatedStocks[existingIndex] = updatedStock;
                    return updatedStocks;
                }
                return [...prev, updatedStock];
            });
        }
    };



    const handleNext = () => {
        if (selectedStocks.length > 0 && error === "") {
            onSelectStocks(selectedStocks);
        }
        else {
            alert("אנא בחר מוצרים להמשך");
        }
    };
    return (
        <div style={{ padding: "20px", maxWidth: "800px", margin: "auto" }}>
            <Typography
                variant="h5"
                align="center"
                style={{ marginBottom: "20px", color: "#1976d2" }}
            >
                בחר סחורות מהספק
            </Typography>

            {stocks && stocks.length > 0 && stocks.map((stock) => (
                <div
                    key={stock.id}
                    style={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        marginBottom: "15px",
                    }}
                >
                    <div style={{ flex: 1 }}>
                        <Typography
                            variant="body1"
                            color="primary"
                            style={{ fontWeight: "bold" }}
                        >
                            {stock.name}
                        </Typography>
                        <Typography variant="body2" color="textSecondary">
                            מחיר: ₪{stock.price} | כמות מינימלית: {stock.minAmount}
                        </Typography>
                    </div>

                    <div style={{ display: "flex", flexDirection: "column" }}>
                        <TextField
                            type="number"
                            label="בחר כמות"
                            variant="outlined"
                            size="small"
                            inputProps={{ min: stock.minAmount }}
                            onChange={(e) => { if (e.target.value) { handleStockSelection(stock.id, parseInt(e.target.value)) } }}
                            style={{ width: "100px" }}
                        />
                    </div>
                </div>
            ))}

            {error && (
                <Typography color="error" align="center" style={{ marginBottom: "20px" }}>
                    {error}
                </Typography>
            )}

            <Typography
                variant="h6"
                align="right"
                style={{ marginTop: "20px", color: "#1976d2" }}
            >
                סך הכול: ₪{totalPrice}
            </Typography>

            <div style={{ marginTop: "20px", textAlign: "center" }}>
                <Button
                    variant="contained"
                    color="primary"
                    onClick={handleNext}
                    disabled={selectedStocks.length === 0}
                    style={{
                        width: "100%",
                        padding: "12px",
                        fontSize: "16px",
                        textTransform: "none",
                        borderRadius: "5px",
                        backgroundColor: "#1976d2",
                        color: "#fff",
                    }}
                >
                    שלב הבא
                </Button>
            </div>
        </div>
    );
};

