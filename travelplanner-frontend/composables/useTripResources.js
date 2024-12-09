import {environmentAPI} from "~/utils/environments/environmentAPI.js";
import {apiPaths} from "~/utils/paths/apiPaths.js";
import {requestOptionsHelper} from "~/utils/requestOptionsHelper.js";
import {Trip} from "~/model/trip";

export const useTripResources = () => {
    const baseUrl = environmentAPI.baseUrl;

    const urlTrip = `${baseUrl}/${apiPaths.trips}`;

    const getTrip = async () => {
        try {
            const response = await fetch(urlTrip, requestOptionsHelper.get);
            const data = await response.json();

            return data.map(item => new Trip(item));
        } catch (error) {
            console.error('Error during getting trips', error);
            throw error;
        }
    };


    const postTrip = async (credentials) => {
        try {
            const response = await fetch(urlTrip, requestOptionsHelper.post(credentials));
            if (response.ok) {
                return {status: response.status, message: 'Trips successfully created'};
            } else {
                return {status: response.status, message: 'Could not create a trip.'};
            }
        } catch(error) {
            console.error('Error during creating a trip', error);
            throw error;
        }
    }

    const putTrip = async (credentials) => {
        try {
            const response = await fetch(urlTrip, requestOptionsHelper.put(credentials));
            if (response.ok) {
                return {status: response.status, message: 'Trip successfully updated'};
            } else {
                return {status: response.status, message: 'Could not updated a trip.'};
            }
        } catch(error) {
            console.error('Error during updating a trip', error);
            throw error;
        }
    }

    const deleteTrip = async () => {
        try {
            const response = await fetch(urlTrip, requestOptionsHelper.del);
            if (response.ok) {
                const data = response.json();
                console.log(data);
            } else {
                const errorData = await response.json();
                throw new Error(errorData.message || 'Could not delete trip.');
            }
        } catch(error) {
            console.error('Error during deleting a trip', error);
            throw error;
        }
    }

    return{
        getTrip,
        postTrip,
        putTrip,
        deleteTrip
    };

}
