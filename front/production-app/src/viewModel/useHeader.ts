import { useEffect, useState } from "react";
import Info from "../pages/InfoPage";

// Models
import type { machine } from "../model/machine";

function useHeader() {
    const [state, setState] = useState<boolean>(false);
    
    async function sendState(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/production/state/${state}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });
        console.log("Fetched:", res);
    }

    async function changeState(): Promise<void> {
        setState(!state);
    }

    return { state, changeState, sendState }
}

export default useHeader;