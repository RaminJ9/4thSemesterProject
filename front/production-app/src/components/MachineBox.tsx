import "./CSS/MashineBox.css"

import type { machine } from "../model/machine";

type MachineBoxProps = {
    machineInfo: machine;
};

function MachineBox({ machineInfo }: MachineBoxProps) {


    return (
        <div id="Box">
            <p>{machineInfo.name}</p>
            <p>{machineInfo.id}</p>
        </div>
    );
}

export default MachineBox;