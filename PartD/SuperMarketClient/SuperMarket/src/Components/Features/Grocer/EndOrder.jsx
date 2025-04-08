import React from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import {  Button, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow,Paper,} from "@mui/material";
import { ExpandRounded } from "@mui/icons-material";

export default function EndOrder({ selectedStocks, supplierId }){
  const navigate = useNavigate();
  const calculateTotalPrice = () => {
    return selectedStocks.reduce((total, stock) => {
      return total + stock.quantity * stock.price;
    }, 0);
  };

  const handleSubmit = async () => {
    const orderData = {
      supplierId: supplierId,
      total: calculateTotalPrice(),
      date: new Date().toISOString(),
      status: "בעיבוד",
      userId:1
    };

    try {
      const orderResponse = await axios.post("https://localhost:7270/api/Order", orderData,
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("token")}`,
          },
        }
      );
      const orderId = orderResponse.data.id; 

      for (const stock of selectedStocks) {
        const item = {
          orderId: orderId,
          stockId: stock.productId,
          name: stock.name,
          quantity: stock.quantity
        };
        await axios.post("https://localhost:7270/api/ProductOrder", item);
      }

      alert("ההזמנה נשלחה בהצלחה!");

    } catch (error) {
      console.error("שגיאה בשליחת ההזמנה או הפריטים:", error);
      alert("אירעה שגיאה בעת שליחת ההזמנה. נסי שוב.");
    }
    setTimeout(() => {
      navigate("/home"); 
    }, 2000); 
  };

  return (
    <div style={{ padding: "20px", maxWidth: "800px", margin: "auto" }}>
      <Typography
        variant="h5"
        align="center"
        style={{ marginBottom: "20px", color: "#1976d2" }}
      >
        סיכום הזמנה
      </Typography>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>שם סחורה</TableCell>
              <TableCell align="right">כמות</TableCell>
              <TableCell align="right">מחיר ליחידה (₪)</TableCell>
              <TableCell align="right">מחיר כולל (₪)</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {selectedStocks.map((stock) => (
              <TableRow key={stock.productId}>
                <TableCell>{stock.productName}</TableCell>
                <TableCell align="right">{stock.quantity}</TableCell>
                <TableCell align="right">{stock.price}</TableCell>
                <TableCell align="right">
                  {stock.quantity * stock.price}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      <Typography
        variant="h6"
        align="right"
        style={{ marginTop: "20px", color: "#1976d2" }}
      >
        סך הכל: ₪{calculateTotalPrice()}
      </Typography>

      <div style={{ marginTop: "20px", textAlign: "center" }}>
        <Button
          variant="contained"
          color="primary"
          onClick={handleSubmit}
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
          ביצוע הזמנה
        </Button>
      </div>
    </div>
  );
};

