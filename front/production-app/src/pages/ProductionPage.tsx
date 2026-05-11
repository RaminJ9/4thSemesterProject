import "./CSS/ProductionLine.css"
import Header from "../components/Header";
import MachineBox from "../components/MachineBox";

import type { machine } from "../model/machine";
import { useState } from "react";

function Production() {
  const [productionLine, setProductionLine] = useState<string[][]>([]);

  return (
    <>
      <Header />
      <h1>Production Page</h1>

      <div id="production">
        { productionLine.length === 0 ?
          <div className="ProductionIndex"></div>
          :
          productionLine.map( step =>
            <div className="ProductionIndex">
              <MachineBox name={step[0]} />

              <button   
                onClick={() => {
                  setProductionLine(prev => [...prev, ["b"]]);
                }}
              >
                +
              </button>
            </div>
            
          )
        }

    
        <button   
          onClick={() => {
            setProductionLine(prev => [...prev, ["a"]]);
          }}
        >
          +
        </button>
      </div>  
    </>
  );
}

export default Production;