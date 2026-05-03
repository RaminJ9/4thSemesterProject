import "./CSS/ProductionBox.css"
import MachineBox from "./MachineBox";

function ProductionBox() {
    return (
        <div id="ProductionBox">
            <MachineBox />
            <MachineBox />
            <button>+</button>
        </div>
    );
}

export default ProductionBox;