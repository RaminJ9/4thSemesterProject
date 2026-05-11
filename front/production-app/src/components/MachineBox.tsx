import "./CSS/MashineBox.css"

// import type { machine } from "../model/machine";


type MachineBoxProps = {
  name: string;
};

function MachineBox({ name }: MachineBoxProps) {

    return (
        <div id="Box">
            <p>{name}</p>
        </div>
    );
}

export default MachineBox;