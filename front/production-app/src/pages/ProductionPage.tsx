import "./CSS/ProductionLine.css"
import { useState, useEffect } from "react";
import Header from "../components/Header";
import MachineBox from "../components/MachineBox";

import useProductionPage from "../viewModel/useProductionPage";

function Production() {
  const { machines, production, saveProduction, message } = useProductionPage();

  const [productionLine, setProductionLine] = useState<string[][]>([]);
  const [selectedMachine, setSelectedMachine] = useState<string>("");  

  const [succes, setSucces] = useState<string>("");
  const showSucces = (text: string) => {
      setSucces(text);
      setTimeout(() => {
      setSucces("");
      }, 6000);
  };
  
  useEffect(() => {
    if (production) {
      setProductionLine(production);
    }
  }, [production]);
  
  return (  
    <>
      <Header />
      <div id="page">
        {message && // if message exist
          <div className="error">
            <p>{message}</p>
          </div>
        }

        {succes &&
          <div className="succes">
            <p>{succes}</p>
          </div>
        }

        <div id="production">
          { productionLine.length === 0 ?

            <div className="ProductionIndex"></div>
            :
            
            productionLine.map((indexInfo, indexStep) => (
              <div key={indexStep} className="ProductionIndex">
                <div className="box">
                  <h3>{indexStep}</h3>

                  {productionLine[indexStep].map( guid => 
                    <MachineBox guid={guid} />
                  )}
                </div>
              
                <div id="action">
                  <select
                    value={selectedMachine}
                    onChange={(e) => {
                      setSelectedMachine(e.target.value);
                      setProductionLine(prev =>
                        prev.map((step, index) =>
                          index === indexStep
                            ? [...step, e.target.value]
                            : step
                        )
                      ); 
                      setSelectedMachine("");
                    }}
                  >
                    <option value="">+</option>
                    {machines
                    .filter(machine => !indexInfo.includes(machine.guid))
                    .map((machine) => (
                      <option
                        key={machine.guid}
                        value={machine.guid}
                      >
                        {machine.name}
                      </option>
                    ))}
                  </select>
                  
                  <button onClick={ () => {
                    setProductionLine(prev =>
                      prev.filter((_, i) => i !== indexStep)
                    );
                  }}>
                    Delete
                  </button>
                </div>
              
              </div> 
            ))
          }

          <select
            value={selectedMachine}
            onChange={(e) => {
              setSelectedMachine(e.target.value);
              setProductionLine(prev => [
                ...prev,
                [e.target.value]
              ]);
              setSelectedMachine("");
            }}
          >
            <option value="">+</option>
            {machines.map((machine) => (
              <option
                key={machine.guid}
                value={machine.guid}
              >
                {machine.name}
              </option>
            ))}
          </select>

        </div>
        <button
          onClick={async () => {
            const result = await saveProduction(productionLine);
            if (result) { showSucces("Pruduction saved") }
          }}
        >
          Save
        </button>
      </div>
      
    </>
  );
}

export default Production;