import React from 'react';

class AgendamentoLista extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      agendamentoLista: []
    }
  }

  carregarDados() {
    const apiUrl = 'https://localhost:44340/api/agendamento';

    fetch(apiUrl)
      .then(res => res.json())
      .then(
        (result) => {
          this.setState({
            agendamentoLista: result
          });
        },
        (error) => {
          this.setState({ error });
          console.error(error);
        }
      )
  }

  componentDidUpdate(prevProps) {
    if (prevProps.mensagemFormulario !== this.props.mensagemFormulario) {
      this.carregarDados();
    }
  }

  render() {
    const { error, agendamentoLista} = this.state;

    if(error) {
      return (
        <div>Error: {error.message}</div>
      )
    } else {
      return(
        <div>
          <h2>Agendamento Lista</h2>
          <h3>Situacao: {this.props.mensagemFormulario}</h3>
          <table>
            <thead>
              <tr>
                <th>Carreta</th>
                <th>Fornecerdo</th>
                <th>Inicio</th>
                <th>Termino</th>
                <th>Vaga</th>
              </tr>
            </thead>
            <tbody>
              {agendamentoLista.map((agendamento, index) => (
                <tr key={index}>
                  <td>{agendamento.agendamentoCarreta}</td>
                  <td>{agendamento.agendamentoFornecedor}</td>
                  <td>{agendamento.agendamentoInicio}</td>
                  <td>{agendamento.agendamentoTermino}</td>
                  <td>{agendamento.agendamentoVaga}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )
    }
  }
}

export default AgendamentoLista;