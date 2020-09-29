import React from 'react';
import logo from './logo.svg';
import './App.css';
import AgendamentoLista from './Agendamento/AgendamentoLista';
import AgendamentoFormulario from './Agendamento/AgendamentoFormulario';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        Recebimento de Mercadorias - Agendamento
      </header>
      <AgendamentoLista />
      <AgendamentoFormulario />
    </div>
  );
}

export default App;
