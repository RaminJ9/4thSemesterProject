import { MyRoutes } from "../Routes";
import "./CSS/ProductionLine.css"
import Header from "../components/Header";
import ProductionBox from "../components/ProductionBox";

function Production() {
  return (
    <body>
      <Header />
      <h1>Production Page</h1>
      <div id="ProductionLine">
        <ProductionBox />
        <ProductionBox />
        <button>+</button>
      </div>
    </body>
  );
}

export default Production;