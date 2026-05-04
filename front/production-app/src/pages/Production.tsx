import "./CSS/ProductionLine.css"
import Header from "../components/Header";
//import ProductionBox from "../components/ProductionBox";
import MachineBox from "../components/MachineBox";

import type { machine } from "../model/machine";

function Production() {
  // Remove mock then impplementet:
  // const { machines } = useProduction();

  // mock -------------------------------------
  const machine1: machine = {
    id: 1,
    name: "number1",
    connectionString: "wow",
    attributes: []
  };
    const machine2: machine = {
    id: 2,
    name: "number1",
    connectionString: "wow",
    attributes: []
  };

  const machines: machine[] = [machine1, machine2]
  // mock -------------------------------------

  return (
    <body>
      <Header />
      <h1>Production Page</h1>
      <div id="ProductionLine">
        <div id="ProductionBox">
          {machines.map( machine =>
            <MachineBox key={machine.id} machineInfo={machine}/> // Type '{ machineInfo: machine; }' is not assignable to type 'IntrinsicAttributes & machine'.  Property 'machineInfo' does not exist on type 'IntrinsicAttributes & machine'.
          )}
          <button>+</button>
        </div>
        <button>+</button>
      </div>
    </body>
  );
}

export default Production;