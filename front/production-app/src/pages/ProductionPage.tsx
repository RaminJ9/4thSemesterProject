import "./CSS/ProductionLine.css"
import Header from "../components/Header";
//import ProductionBox from "../components/ProductionBox";
import MachineBox from "../components/MachineBox";

import type { machine } from "../model/machine";

function Production() {
  // Remove mock then impplementet:
  // const { machines } = useProduction();
  const 
  
  return (
    <>
      <Header />
      <div id="production">
        <h1>Production Page</h1>
        <div id="ProductionLine">
          <div id="ProductionBox">
            <button>+</button>
          </div>
          <button>+</button>
        </div>
      </div>
    </>
  );
}

export default Production;