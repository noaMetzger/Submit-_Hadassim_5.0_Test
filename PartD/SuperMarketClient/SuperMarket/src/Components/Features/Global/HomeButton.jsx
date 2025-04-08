import { useNavigate } from "react-router-dom";
import HomeIcon from "@mui/icons-material/Home";
import { Snackbar, Alert, Button, Box } from "@mui/material";


//אייקון של חזרה לעמוד הבית

export default function HomeButton() {
    const navigate = useNavigate();
    return (
      <Button
        onClick={() =>{ navigate("/home");}}
        sx={{
          height: '10vh',
          width: "auto",
          position: "fixed",
          top: 20,
          right: 20,
          backgroundColor: "white",
          borderRadius: "50%",
          width: "50px",
          height: "50px",
          minWidth: "0",
          padding: "0",
          boxShadow: "0px 2px 5px rgba(0, 0, 0, 0.2)",
          "&:hover": {
            backgroundColor: "#f0f0f0",
          },
          zIndex: 1000,
        }}
      >
        <HomeIcon sx={{ color: "black", fontSize: 30 }} />
      </Button>
    );
  }