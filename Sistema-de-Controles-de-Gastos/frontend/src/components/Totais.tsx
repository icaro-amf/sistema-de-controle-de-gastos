import type { TotaisResponse } from "../types";
import { formatarMoeda } from "../utils/format";

interface Props {
  totais: TotaisResponse;
}

function Totais({ totais }: Props) {
  return (
    <div>
      <h2>Resumo Financeiro</h2>

      <div className="totais-grid">
        <div className="total-card receita">
          <div className="total-card-label">Receitas</div>
          <div className="total-card-valor">{formatarMoeda(totais.totalGeralReceitas)}</div>
        </div>
        <div className="total-card despesa">
          <div className="total-card-label">Despesas</div>
          <div className="total-card-valor">{formatarMoeda(totais.totalGeralDespesas)}</div>
        </div>
        <div className="total-card saldo">
          <div className="total-card-label">Saldo</div>
          <div className="total-card-valor">{formatarMoeda(totais.saldoGeral)}</div>
        </div>
      </div>

      {totais.totaisPorPessoa.length > 0 && (
        <>
          <h3>Por pessoa</h3>
          <div className="totais-por-pessoa">
            {totais.totaisPorPessoa.map((p) => (
              <div key={p.pessoaId} className="total-pessoa-row">
                <span className="total-pessoa-nome">{p.nome}</span>
                <div className="total-pessoa-valores">
                  <span>
                    Receitas: <strong>{formatarMoeda(p.totalReceitas)}</strong>
                  </span>
                  <span>
                    Despesas: <strong>{formatarMoeda(p.totalDespesas)}</strong>
                  </span>
                  <span>
                    Saldo: <strong>{formatarMoeda(p.saldo)}</strong>
                  </span>
                </div>
              </div>
            ))}
          </div>
        </>
      )}
    </div>
  );
}

export default Totais;
