import { Link } from "react-router-dom";
import { MyRoutes } from "../Routes";
import useHeader from "../viewModel/useHeader";

// Style
import "./CSS/Header.css"

function Header() {
  const { state, changeState, sendState } = useHeader();
  return (
    <header>
      <div id="left">
        <button
          onClick={ async () => {
            await changeState();
            await sendState();
          }}
        >
          { state ? <p>Pause</p> : <p>Turn on</p>}
        </button>
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