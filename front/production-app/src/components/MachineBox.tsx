import "./CSS/MashineBox.css"

// import type { machine } from "../model/machine";

import useProductionPage from "../viewModel/useProductionPage";


type MachineBoxProps = {
  guid: string;
};

function MachineBox({ guid }: MachineBoxProps) {
    const {machines} = useProductionPage();
    const machine = machines.find(
        machine => machine.guid === guid
    )
    return (
        <div id="Box">
            <p>{machine?.name}</p>
            <p id="guid">{machine?.guid}</p>
        </div>
    );
}

export default MachineBox;