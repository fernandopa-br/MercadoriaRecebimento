import React from 'react';
import AgendamentoLista from './AgendamentoLista';

class AgendamentoFormulario extends React.Component {
  constructor(props) {
    super(props);
    this.handleChange = this.handleChange.bind(this);
    this.state = {
      error: null,
      agendamentoCarreta: "Carreta",
      agendamentoFornecedor: "Fornecedor",
      agendamentoData: "2020-09-29",
      agendamentoInicio: "08:00",
      agendamentoTermino: "09:00",
      agendamentoVaga: "1",
      responseMensagem: "Inicializado"
    }
  }

  agendamentoAdicionar = (e) => {
    const { agendamentoCarreta, agendamentoFornecedor, agendamentoData, agendamentoInicio, agendamentoTermino, agendamentoVaga } = this.state;
    var agendamentoRegisto = {
      agendamentoCarreta: agendamentoCarreta,
      agendamentoFornecedor: agendamentoFornecedor,
      agendamentoInicio: agendamentoData + "T" + agendamentoInicio + ":00-03:00",
      agendamentoTermino: agendamentoData + "T" + agendamentoTermino + ":00-03:00",
      agendamentoVaga: agendamentoVaga
    }
    const apiUrl = 'https://localhost:44340/api/agendamento';
    fetch(apiUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(agendamentoRegisto)
    })
      .then(res => res.json())
      .then(
        (result) => {
          this.setState({
            responseMensagem: result.value
          });
        },
        (error) => {
          this.setState({ error });
          console.error(error);
        }
      )
    e.preventDefault();
  }

  handleChange(e) {
    const target = e.target;
    const value = target.type === 'checkbox' ? target.checked : target.value;
    const name = target.name;

    this.setState({
      [name]: value
    });
  }


  render() {
    const { error, agendamentoCarreta, agendamentoFornecedor, agendamentoData, agendamentoInicio, agendamentoTermino, agendamentoVaga } = this.state;
    if (error) {
      console.error(error);
      return (
        <div>Error: {error.message}</div>
      )
    } else {
      return (
        <div>
          <h2>Agendamento Formulario</h2>
          <table>
            <thead>
              <tr>
                <th>Carreta</th>
                <th>Fornecedor</th>
                <th>Data</th>
                <th>Inicio</th>
                <th>Termino</th>
                <th>Vaga</th>
                <th>&nbsp;</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td><input type="text" name="agendamentoCarreta" value={agendamentoCarreta} onChange={this.handleChange} /></td>
                <td><input type="text" name="agendamentoFornecedor" value={agendamentoFornecedor} onChange={this.handleChange} /></td>
                <td><input type="text" name="agendamentoData" value={agendamentoData} onChange={this.handleChange} /></td>
                <td><input type="text" name="agendamentoInicio" value={agendamentoInicio} onChange={this.handleChange} /></td>
                <td><input type="text" name="agendamentoTermino" value={agendamentoTermino} onChange={this.handleChange} /></td>
                <td><input type="text" name="agendamentoVaga" value={agendamentoVaga} onChange={this.handleChange} /></td>
                <td><button onClick={this.agendamentoAdicionar}>Adicionar</button></td>
              </tr>
            </tbody>
          </table>
          <AgendamentoLista mensagemFormulario={this.state.responseMensagem} />
        </div>
      )
    }
  }
}

export default AgendamentoFormulario;