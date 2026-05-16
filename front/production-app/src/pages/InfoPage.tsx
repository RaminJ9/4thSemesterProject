import { MyRoutes } from "../Routes";
import { useState } from "react";
import { Link } from "react-router-dom";
import useInfoPage from "../viewModel/useInfoPage";
import { useParams, useNavigate } from "react-router-dom";

// models
import type { machine } from "../model/machine";

// Components
import Header from "../components/Header";

// Style
import "./CSS/InfoPage.css"

function Info() {
  const navigate = useNavigate();

  const { guid } = useParams();
  const { components, machines, addMachine, removeMachine, message } = useInfoPage();

  const [makeMachine, setMakeMachine] = useState<machine>({
    name: "",
    connectionString: "",
    component: ""
  });
  const currentMaschine = machines.find(machine => machine.guid === guid);
  
  return (
    <>
      <Header />
      <main>
        <div id="nav">
          {machines.map( machine =>
            <Link key={machine.guid} to={MyRoutes.machineInfoPath(machine.guid)}>
              <button>{machine.name}</button>
            </Link>
          )}
          <Link to={MyRoutes.info}>
            <button>+</button>
          </Link>
        </div>

        <div id="info">
          {message && // if message exist
            <div className="error">
              <p>{message}</p>
            </div>
          }
          
          {guid === undefined ?
            <div id="config">
              <h1>Configure new machine</h1>
              
              <p>Name:</p>
              <input
                type="text"
                value={makeMachine.name}
                onChange={(e) =>
                  setMakeMachine({
                    ...makeMachine,
                    name: e.target.value
                  })
                }
              />

              <p>Connectionstring:</p>
              <input
                type="text"
                value={makeMachine.connectionString}
                onChange={(e) =>
                  setMakeMachine({
                    ...makeMachine,
                    connectionString: e.target.value
                  })
                }
              />

              <select
                value={makeMachine.component}
                onChange={(e) =>
                  setMakeMachine({
                    ...makeMachine,
                    component: e.target.value
                  })
                }
              >
                <option value="">Component</option>
                {components.map( component => 
                  <option value={component}>{component}</option>
                )}
              </select>

              <button
                className="action"
                onClick={async () => {
                  const result = await addMachine(makeMachine);
                  if (result) { window.location.reload() }
                }}
              >
                Save
              </button>
            </div>
          :
            <div>
              <h1>{currentMaschine?.name}</h1>
              <p>{currentMaschine?.connectionString}</p>
              <p>{currentMaschine?.guid}</p>
              <button className="action" onClick={async () => {
                await removeMachine(guid);
                navigate(MyRoutes.info);
              }}> 
              Delete
              </button>
            </div>
          }
        </div>
      </main>
    </>
  );
}

export default Info;