import React from "react";
import { Button, Box, Typography, Container } from "@mui/material";

export default function SupplierPage() {
  return (
    <Container
      maxWidth="md"
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        // height: "80vh", 
        padding: "2rem",
        borderRadius: "16px",
      }}
    >
      <Typography
        variant="h4"
        sx={{
          fontWeight: "bold",
          color: "#0288D1",
          marginBottom: "2rem",
        }}
      >
        עמוד ספק
      </Typography>
      <Box
        sx={{
          display: "flex",
          gap: "1.5rem",
        }}
      >
        <Button
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
                backgroundColor: "#81D4FA",
            },
          }}
          href="/register"
        >
          הרשמה
        </Button>
        <Button
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
            },
          }}
          href="/login"
        >
          התחברות
        </Button>
      </Box>
    </Container>
  );
}
