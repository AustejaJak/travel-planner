export const headerComponentProps = {
    data: {
        type: Object,
        required: true,
        default: () => ({
            partialNavigation: {
                enabled: false,
            },
        }),
    },
};
