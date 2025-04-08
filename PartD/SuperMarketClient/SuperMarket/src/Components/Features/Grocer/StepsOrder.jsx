import { useState } from "react";
import ChoseSupplier from './ChooseSupplier.jsx';
import ChoseStock from './ChooseStock.jsx';
import EndOrder from "./EndOrder.jsx";

export default function FinalOrder() {
  const [step, setStep] = useState(1);
  const [supplierId, setSupplierId] = useState(null);
  const [selectedStocks, setSelectedStocks] = useState([]);
  
  const handleSelectSupplier = (id) => {
    setSupplierId(id);
    setStep(2); 
  };

  const handleSelectStocks = (stocks) => {
    setSelectedStocks(stocks);
    setStep(3); 
  };

  return (
    <>
      {step === 1 && <ChoseSupplier onSelectSupplier={handleSelectSupplier} />}
      {step === 2 && <ChoseStock supplierId={supplierId} onSelectStocks={handleSelectStocks} />}
      {step === 3 && <EndOrder selectedStocks={selectedStocks} supplierId={supplierId}  />}
    </>
  );
}