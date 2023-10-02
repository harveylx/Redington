import React, { useState } from 'react';
import {
  Tabs,
  Tab,
  Box,
  TextField as MuiTextField,
  Button,
  Typography,
  Grid,
} from '@mui/material';
import { styled } from '@mui/system';
import axios from 'axios';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
});

const combinedWithEndpoint = process.env.REACT_APP_COMBINED_WITH_ENDPOINT;
const eitherEndpoint = process.env.REACT_APP_EITHER_ENDPOINT;

const isValidProbability = (value) => {
  const num = parseFloat(value);
  return !isNaN(num) && num >= 0 && num <= 1;
};

// Create a styled TextField component to hide the spinner
const TextField = styled(MuiTextField)({
  '& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button': {
    '-webkit-appearance': 'none',
    margin: 0,
  },
  '& input[type=number]': {
    '-moz-appearance': 'textfield',
  },
});

function App() {
  const [value, setValue] = useState(0);
  const [probabilityA, setProbabilityA] = useState('');
  const [probabilityB, setProbabilityB] = useState('');
  const [result, setResult] = useState(null);

  const handleCalculate = async () => {
    const action = value === 0 ? combinedWithEndpoint : eitherEndpoint;
    try {
      const response = await api.post(`${action}`, {
        ProbabilityA: probabilityA,
        ProbabilityB: probabilityB,
      });
      setResult(response.data);
    } catch (error) {
      // Handle error messages
      const errors = error.response?.data;
      if (errors && errors.length) {
        // If there are multiple error messages, join them with newline
        const errorMsg = errors.map(err => err.errorMessage).join('\n');
        setResult(`Error: ${errorMsg}`);
      } else {
        setResult(`Error: ${error.message || 'Unknown Error'}`);
      }
    }
  };

  return (
    <Grid
      container
      direction="column"
      justifyContent="center"
      alignItems="center"
      style={{ minHeight: '100vh' }}
    >
      <Tabs
        value={value}
        onChange={(e, newValue) => setValue(newValue)}
        centered
      >
        <Tab label="Combined With" />
        <Tab label="Either" />
      </Tabs>
      <Box my={2}>
        <TextField
          error={!isValidProbability(probabilityA)}
          helperText
          label="Probability A"
          variant="outlined"
          value={probabilityA}
          onChange={(e) => {
            if (!isNaN(e.target.value)) {
              setProbabilityA(e.target.value);
            }
          }}
          sx={ { marginRight: "1rem" }}
        />
        <TextField
          error={!isValidProbability(probabilityB)}
          label="Probability B"
          variant="outlined"
          value={probabilityB}
          onChange={(e) => {
            if (!isNaN(e.target.value)) {
              setProbabilityB(e.target.value);
            }
          }}
        />
      </Box>
      <Button variant="contained" color="primary" onClick={handleCalculate} disabled={!isValidProbability(probabilityA) || !isValidProbability(probabilityB)}>
        Calculate
      </Button>
      {result !== null && (
        <Typography variant="h5" style={{ marginTop: '16px' }}>
          Result: {result}
        </Typography>
      )}
    </Grid>
  );
}

export default App;
