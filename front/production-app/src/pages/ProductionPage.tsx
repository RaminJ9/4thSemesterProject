import "./CSS/ProductionLine.css"
import { useState, useEffect } from "react";
import Header from "../components/Header";
import MachineBox from "../components/MachineBox";

import useProductionPage from "../viewModel/useProductionPage";

function Production() {
  const { machines, production, saveProduction } = useProductionPage();

  const [productionLine, setProductionLine] = useState<string[][]>([]);

  const [selectedMachine, setSelectedMachine] = useState("");  
  
  useEffect(() => {
  if (production) {
    setProductionLine(production);
  }
}, [production]);
  return (  
    <>
      <Header />
      <div>
        <div id="production">
          { productionLine.length === 0 ?

            <div className="ProductionIndex"></div>
            :
            
            productionLine.map((indexInfo, indexStep) => (
              
              <div key={indexStep} className="ProductionIndex">
                <div className="box">
                  <p>{indexStep}</p>

                  {productionLine[indexStep].map( line => 
                    <MachineBox guid={line} />
                  )}
                </div>
              
                <div>
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
                    {machines.map((machine) => (
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
            await saveProduction(productionLine);
            window.location.reload();
          }}
        >
          Save
        </button>
      </div>



      <p>{production}</p> 
      <p>{productionLine}</p>
    </>
  );
}

export default Production;