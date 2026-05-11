import { useEffect, useState } from "react";
import Info from "../pages/InfoPage";

// Models
import type { machine } from "../model/machine";

function useProductionPage() {
    const [machines, setMachines] = useState<machine[]>([]);

    async function fetchMachines(): Promise<void> {
        const res = await fetch(`http://localhost:5253/api/machine`, {
            method: "GET"
        });
        console.log("Fetched:", res);
    
        setMachines(await res.json());
    }

    return { machines }
}

export default useProductionPage;