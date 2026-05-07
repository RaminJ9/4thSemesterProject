import { Link } from "react-router-dom";
import { MyRoutes } from "../Routes";

// Style
import "./CSS/Header.css"

function Header() {
  return (
    <header>
      <div id="left">
        <button>start</button>
      </div>
      <div id="right">
        <Link to={MyRoutes.production}>
          <button>Production</button>
        </Link>
        <Link to={MyRoutes.info}>
          <button>Info/configuration</button>
        </Link>
      </div>
    </header>
  );
}

export default Header;