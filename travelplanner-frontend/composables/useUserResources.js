import { requestOptionsHelper } from "~/utils/requestOptionsHelper.js";
import { User } from "~/model/user.js";

export const useUserResources = () => {
    const urlUsers = `http://localhost:5207/api/v1/user?pageNumber=1&pageSize=5`;

    const getUsers = async () => {
        try {
            const response = await fetch(urlUsers, requestOptionsHelper.get);
            const data = await response.json();

            return data.map(item => new User(item));
        } catch (error) {
            console.error('Error during getting users', error);
            throw error;
        }
    };

    const removeUserRoles = async (userId) => {
        const urlPostUser = `http://localhost:5207/api/v1/user/removeAllRoles/${userId}`;
        try {
            const response = await fetch(urlPostUser, requestOptionsHelper.post({}));
            if (response.ok) {
                return { status: response.status, message: 'User roles successfully updated to TravelMember' };
            } else {
                return { status: response.status, message: 'Could not update user roles.' };
            }
        } catch (error) {
            console.error('Error during updating user roles', error);
            throw error;
        }
    }

    const assignAdminRole = async (userId) => {
        const urlAssignAdmin = `http://localhost:5207/api/v1/user/assignAdmin/${userId}`;
        try {
            const response = await fetch(urlAssignAdmin, requestOptionsHelper.post({}));
            if (response.ok) {
                return { status: response.status, message: 'User role successfully updated to Admin' };
            } else {
                return { status: response.status, message: 'Could not update user role to Admin.' };
            }
        } catch (error) {
            console.error('Error during assigning admin role', error);
            throw error;
        }
    }

    const deleteUser = async (userId) => {
        const urlDeleteUser = `http://localhost:5207/api/v1/user/deleteUser/${userId}`;
        try {
            const response = await fetch(urlDeleteUser, requestOptionsHelper.del);
            if (response.ok) {
                return { status: response.status, message: 'User successfully deleted' };
            } else {
                return { status: response.status, message: 'Could not delete user.' };
            }
        } catch (error) {
            console.error('Error during deleting user', error);
            throw error;
        }
    }


    return {
        getUsers,
        removeUserRoles,
        assignAdminRole,
        deleteUser
    };
};
