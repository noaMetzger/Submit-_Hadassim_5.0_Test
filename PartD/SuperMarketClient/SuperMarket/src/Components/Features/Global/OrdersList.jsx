
import { useState, useEffect } from "react";
import { Box, Typography, Grid, CircularProgress, Alert, Button, Collapse, Paper } from "@mui/material";
import axios from "axios";
import HomeButton from "./HomeButton";


export default function ShowOrdersById() {

    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [role, setRole] = useState("");
    const [openOrderDetails, setOpenOrderDetails] = useState({});

    // פונקציה  להחזרת צבעים אקראיים
    const getRandomColor = () => {
        const colors = ["#FFEB3B", "#4CAF50", "#2196F3", "#FF5722", "#9C27B0"];
        return colors[Math.floor(Math.random() * colors.length)];
    };

    function parseJwt(token) {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split("")
                .map(function (c) {
                    return "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2);
                })
                .join("")
        );
        return JSON.parse(jsonPayload);
    }

    
    useEffect(() => {

        const token = localStorage.getItem('token');
        if (!token) {
             setError('יש לבצע התחברות');
             return;
         }
         const d=parseJwt(token)
         setRole(d.role)
         const fetchOrders = async () => {
         const token = localStorage.getItem('token');

         try {
            const url= d.role ==="grocer" ? "https://localhost:7270/api/Order/orderuser":"https://localhost:7270/api/Order/ordersupplier"
            const response = await axios.get(url, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },})
                setOrders(response.data);
                setLoading(false);
            }
                catch (err) {
                    setError('שגיאה בשרת, נסה שוב מאוחר יותר.');
                }
        };

        fetchOrders();   
    }, []);


    const handleStatusChange = async (orderId) => {
        
        const token = localStorage.getItem('token');
        if (!token) {
            setError('יש לבצע התחברות');
            return;
        }
        let statusOrder="הושלמה"
        try {     
            if(role=="supplier")
                statusOrder="בתהליך"
            debugger
            const response = await axios.put(`https://localhost:7270/api/Order/${orderId}`,null , {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json", 
                },      
                params: {
                    status:statusOrder ,
                },
            });
            setOrders((prevOrders) =>
                prevOrders.map((order) =>
                    //נעדכן לתצוגה העכשוית את ההזמנה המתאימה שיהיה לה סטטוס מעודכן
                    order.id === orderId ? { ...order, status: statusOrder } : order
                )
            );
        } catch (err) {
            setError('שגיאה בעדכון ההזמנה, נסה שוב מאוחר יותר.');
            console.error(err);
        };
    };


        const handleToggleDetails = async (orderId) => {
            setOpenOrderDetails((prevState) => ({
                ...prevState,
                [orderId]: !prevState[orderId],
            }));


        };

        return (
            
            <Box
                sx={{
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    flexDirection: "column",
                    margin: 2,
                }}
            >    <HomeButton />

                {error && (
                    <Alert severity="error" sx={{ width: "60%", textAlign: "center" }}>
                        {error}
                    </Alert>
                )}

                {!loading && !error && orders.length === 0 && (
                    <Typography variant="h6" color="textSecondary">
                        לא נמצאו הזמנות למשתמש הזה.
                    </Typography>
                )}

                {role.trim()!==""&&role && !loading && !error && orders.length > 0 &&  (
                    <>
                        <Typography variant="h5" marginBottom="20px" gutterBottom>
                            הנה ההזמנות שלך
                        </Typography>
                        <Grid container spacing={3} justifyContent="center" sx={{ maxWidth: "80%" }}>
                            {orders.map((order) => (
                                <Grid item xs={12} sm={6} md={4} key={order.id}>
                                    <Paper
                                        sx={{
                                            padding: 2,
                                            borderRadius: "30px",
                                            border: `2px solid ${getRandomColor()}`,
                                            backgroundColor: "transparent",
                                            height: "auto",
                                            display: "flex",
                                            flexDirection: "column",
                                            justifyContent: "space-between",
                                        }}
                                    >
                                        <Typography variant="body1">
                                            <strong>תאריך ההזמנה:</strong> {order.date.split('T')[0]}
                                        </Typography>
                                        <Typography variant="body1">
                                            <strong>סך הכל:</strong> ₪{order.total}
                                        </Typography>
                                        <Typography variant="body1">
                                            <strong>סטטוס:</strong> {order.status}
                                        </Typography>
                                        {role=="grocer"&& <Typography variant="body1">
                                            <strong>מספר ספק:</strong> {order.supplierId}
                                        </Typography>} 
                                        {role=="supplier"&& <Typography variant="body1">
                                            <strong>מספר בעל מכולת:</strong> {order.userId}
                                        </Typography>} 
                                        <Button
                                            variant="outlined"
                                            color="primary"
                                            onClick={() => handleToggleDetails(order.id)}
                                            sx={{
                                                marginTop: 2,
                                                padding: "6px 12px",
                                                fontSize: "0.9rem",
                                                borderColor: "black",
                                                borderRadius: "20px",
                                                color: "black",
                                                ":hover": {
                                                    backgroundColor: "transparent",
                                                    borderColor: "#1976d2",
                                                },
                                            }}
                                        >
                                            צפה בפרטי ההזמנה
                                        </Button>
                                        {((order.status ==="בתהליך"&&role=="grocer")||(order.status == "בעיבוד"&&role=="supplier") )&& <Button
                                            variant="outlined"
                                            color="primary"
                                            onClick={() => handleStatusChange(order.id)}
                                            sx={{
                                                marginTop: 2,
                                                padding: "6px 12px",
                                                fontSize: "0.9rem",
                                                borderColor: "black",
                                                borderRadius: "20px",
                                                color: "black",
                                                ":hover": {
                                                    backgroundColor: "transparent",
                                                    borderColor: "#1976d2",
                                                },
                                            }}
                                        >
                                            אישור הזמנה
                                        </Button>}
                                        <Collapse in={openOrderDetails[order.id]} timeout="auto" unmountOnExit>
                                            <Box
                                                sx={{
                                                    marginTop: 2,
                                                    padding: 2,
                                                    border: "1px solid #ccc",
                                                    borderRadius: 1,
                                                    backgroundColor: "#f9f9f9",
                                                }}
                                            >
                                                <Typography variant="h6">פרטי ההזמנה:</Typography>
                                                {order.products && order.products.length > 0 ? (
                                                    order.products.map((product, index) => (
                                                        <Paper
                                                            key={index}
                                                            sx={{
                                                                padding: 1,
                                                                marginBottom: 1,
                                                                borderRadius: "10px",
                                                                backgroundColor: "#e3f2fd",
                                                            }}
                                                        >
                                                            <Typography variant="body2">
                                                                <strong>שם מוצר:</strong> {product.name}
                                                            </Typography>
                                                            <Typography variant="body2">
                                                                <strong>כמות:</strong> {product.quantity}
                                                            </Typography>
                                                        </Paper>
                                                    ))
                                                ) : (
                                                    <Typography variant="body2">אין מוצרים זמינים להזמנה זו.</Typography>
                                                )}
                                            </Box>
                                        </Collapse>
                                    </Paper>
                                </Grid>
                            ))}
                        </Grid>
                    </>
                )}

            </Box>
        );

    };

