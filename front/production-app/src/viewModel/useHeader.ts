import { useEffect, useState } from "react";
import Info from "../pages/InfoPage";

function useHeader() {
    const [state, setState] = useState<boolean>(false);
    
    async function sendState(): Promise<boolean> {
        const res = await fetch(`http://localhost:5253/api/production/state/${state}`, {
            method: "POST",
            headers: { "Content-Type": "application/json" }
        });
        console.log("Fetched:", res);
        
        if (!res.ok) {
            return false;
        }
        return true
    }

    async function changeState(): Promise<void> {
        setState(!state);
    }

    return { state, changeState, sendState }
}

export default useHeader;