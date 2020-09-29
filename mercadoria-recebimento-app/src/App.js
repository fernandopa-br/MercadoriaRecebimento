import React from 'react';
import logo from './logo.svg';
import './App.css';
import AgendamentoFormulario from './Agendamento/AgendamentoFormulario';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        Recebimento de Mercadorias - Agendamento
      </header>
      <AgendamentoFormulario />
    </div>
  );
}

export default App;
