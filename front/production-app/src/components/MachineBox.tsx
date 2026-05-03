import "./CSS/MashineBox.css"

function MachineBox(name: string, id: number) {
    return (
        <div id="Box">
            <p>{name}</p>
            <p>{id}</p>
        </div>
    );
}

export default MachineBox;