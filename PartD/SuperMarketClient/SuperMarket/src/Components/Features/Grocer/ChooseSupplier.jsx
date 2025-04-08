import React, { useState, useEffect } from "react";
import axios from "axios";
import { Button, Radio, FormControlLabel, Typography } from "@mui/material";

const ChoseSupplier = ({ onSelectSupplier }) => {
  const [suppliers, setSuppliers] = useState([]);
  const [selectedSupplier, setSelectedSupplier] = useState(null);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchSuppliers = async () => {
      try {
        const response = await axios.get("https://localhost:7270/api/Supplier");
        setSuppliers(response.data);
      }
      catch (error) {
        setError("שגיאה בהתחברות לשרת");
      }

    };

    fetchSuppliers();
  }, []);

  const handleNext = () => {
    if (selectedSupplier) {
      onSelectSupplier(selectedSupplier.id);
    } else {
      alert("אנא בחר ספק להמשך");
    }
  };

  return (
    <div style={{ direction: "rtl", padding: '40px', maxWidth: '600px', margin: 'auto', backgroundColor: '#fff', borderRadius: '8px', boxShadow: '0 4px 6px rgba(0, 0, 0, 0.1)' }}>
      <Typography variant="h5" align="center" style={{ marginBottom: '20px', color: '#1976d2' }}>
        בחר ספק להזמנה
      </Typography>

      <div style={{ marginBottom: '20px' }}>
        {suppliers.map(supplier => (
          <FormControlLabel
            key={supplier.id}
            control={<Radio checked={selectedSupplier?.id === supplier.id} />}
            label={supplier.companyName}
            value={supplier.id}
            onChange={() => setSelectedSupplier(supplier)}
            style={{ display: 'block', marginBottom: '10px', direction: 'rtl', textAlign: 'right' }}
          />
        ))}
      </div>

      <Button
        variant="contained"
        color="primary"
        onClick={handleNext}
        disabled={!selectedSupplier}
        style={{
          width: '100%',
          padding: '10px',
          fontSize: '16px',
          textTransform: 'none',
          borderRadius: '5px',
          backgroundColor: '#1976d2',
          color: '#fff',
        }}
      >
        שלב הבא
      </Button>
      {error && (
        <Typography color="error" align="center" style={{ marginBottom: "20px" }}>
          {error}
        </Typography>
      )}
    </div>
  );
};

export default ChoseSupplier;